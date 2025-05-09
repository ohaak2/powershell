using PnP.PowerShell.Commands.Attributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Base.PipeBinds;
using PnP.PowerShell.Commands.Model.Planner;
using PnP.PowerShell.Commands.Utilities;
using PnP.PowerShell.Commands.Utilities.REST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.Planner
{
    [Cmdlet(VerbsCommon.Add, "PnPPlannerTask")]
    [RequiredApiApplicationPermissions("graph/Tasks.ReadWrite")]
    [RequiredApiApplicationPermissions("graph/Tasks.ReadWrite.All")]
    [RequiredApiApplicationPermissions("graph/Group.ReadWrite.All")]
    public class AddPlannerTask : PnPGraphCmdlet
    {
        private const string ParameterName_BYGROUP = "By Group";
        private const string ParameterName_BYPLANID = "By Plan Id";

        [Parameter(Mandatory = true, HelpMessage = "Specify the group id of group owning the plan.", ParameterSetName = ParameterName_BYGROUP)]
        public PlannerGroupPipeBind Group;

        [Parameter(Mandatory = true, HelpMessage = "Specify the id or name of the plan to retrieve the tasks for.", ParameterSetName = ParameterName_BYGROUP)]
        public PlannerPlanPipeBind Plan;

        [Parameter(Mandatory = true, ParameterSetName = ParameterAttribute.AllParameterSets)]
        public PlannerBucketPipeBind Bucket;

        [Parameter(Mandatory = true, ParameterSetName = ParameterName_BYPLANID)]
        public string PlanId;

        [Parameter(Mandatory = true, ParameterSetName = ParameterAttribute.AllParameterSets)]
        public string Title;

        [Parameter(Mandatory = false)]
        public int PercentComplete;

        [Parameter(Mandatory = false)]
        public int Priority;

        [Parameter(Mandatory = false)]
        public DateTime DueDateTime;

        [Parameter(Mandatory = false)]
        public DateTime StartDateTime;

        [Parameter(Mandatory = false)]
        public string Description;

        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets)]
        public string[] AssignedTo;

        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets)]
        public SwitchParameter OutputTask;

        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets)]
        public AppliedCategories AppliedCategories;

        protected override void ExecuteCmdlet()
        {
            PlannerTask createdTask;
            var newTask = new PlannerTask
            {
                Title = Title
            };

            if (ParameterSpecified(nameof(PercentComplete)))
            {
                if (PercentComplete < 0 || PercentComplete > 100)
                {
                    throw new PSArgumentException($"{nameof(PercentComplete)} value must be between 0 and 100.", nameof(PercentComplete));
                }

                newTask.PercentComplete = PercentComplete;
            }

            if (ParameterSpecified(nameof(Priority)))
            {
                if (Priority < 0 || Priority > 10)
                {
                    throw new PSArgumentException($"{nameof(Priority)} value must be between 0 and 10.", nameof(Priority));
                }
                newTask.Priority = Priority;
            }

            if (ParameterSpecified(nameof(StartDateTime)))
            {
                newTask.StartDateTime = StartDateTime.ToUniversalTime();
            }

            if (ParameterSpecified(nameof(DueDateTime)))
            {
                newTask.DueDateTime = DueDateTime.ToUniversalTime();
            }

            if (ParameterSpecified(nameof(AssignedTo)))
            {
                var errors = new List<Exception>();
                newTask.Assignments = new Dictionary<string, TaskAssignment>();
                var chunks = GraphBatchUtility.Chunk(AssignedTo, 20);
                foreach (var chunk in chunks)
                {
                    var userIds = GraphBatchUtility.GetPropertyBatched(GraphRequestHelper, chunk.ToArray(), "/users/{0}", "id");
                    foreach (var userId in userIds.Results)
                    {
                        newTask.Assignments.Add(userId.Value, new TaskAssignment());
                    }
                    if(userIds.Errors.Any())
                    {
                        errors.AddRange(userIds.Errors);
                    }
                }
                if(errors.Any())
                {
                    throw new AggregateException($"{errors.Count} error(s) occurred in a Graph batch request", errors);
                }
            }

            if(ParameterSpecified(nameof(AppliedCategories)))
            {
                newTask.AppliedCategories = AppliedCategories;
            }

            // By Group
            if (ParameterSetName == ParameterName_BYGROUP)
            {
                var groupId = Group.GetGroupId(GraphRequestHelper);
                if (groupId == null)
                {
                    throw new PSArgumentException("Group not found", nameof(Group));
                }

                var planId = Plan.GetId(GraphRequestHelper, groupId);
                if (planId == null)
                {
                    throw new PSArgumentException("Plan not found", nameof(Plan));
                }
                newTask.PlanId = planId;

                var bucket = Bucket.GetBucket(GraphRequestHelper, planId);
                if (bucket == null)
                {
                    throw new PSArgumentException("Bucket not found", nameof(Bucket));
                }
                newTask.BucketId = bucket.Id;

                createdTask = PlannerUtility.AddTask(GraphRequestHelper, newTask);
            }
            // By PlanId
            else
            {
                var bucket = Bucket.GetBucket(GraphRequestHelper, PlanId);
                if (bucket == null)
                {
                    throw new PSArgumentException("Bucket not found", nameof(Bucket));
                }

                newTask.PlanId = PlanId;
                newTask.BucketId = bucket.Id;

                createdTask = PlannerUtility.AddTask(GraphRequestHelper, newTask);
            }

            if (ParameterSpecified(nameof(Description)))
            {
                var existingTaskDetails = PlannerUtility.GetTaskDetails(GraphRequestHelper, createdTask.Id, false);
                PlannerUtility.UpdateTaskDetails(GraphRequestHelper, existingTaskDetails, Description);
                createdTask.HasDescription = true;
            }

            if(OutputTask.IsPresent)
            {
                WriteObject(createdTask);
            }
        }
    }
}