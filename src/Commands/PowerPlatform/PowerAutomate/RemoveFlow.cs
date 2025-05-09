using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Base.PipeBinds;
using PnP.PowerShell.Commands.Utilities;
using System;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.PowerPlatform.PowerAutomate
{
    [Cmdlet(VerbsCommon.Remove, "PnPFlow")]
    public class RemoveFlow : PnPAzureManagementApiCmdlet
    {
        [Parameter(Mandatory = false)]
        public PowerPlatformEnvironmentPipeBind Environment;

        [Parameter(Mandatory = true)]
        public PowerAutomateFlowPipeBind Identity;

        [Parameter(Mandatory = false)]
        public SwitchParameter AsAdmin;

        [Parameter(Mandatory = false)]
        public SwitchParameter ThrowExceptionIfPowerAutomateNotFound;

        [Parameter(Mandatory = false)]
        public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {

            string baseUrl = PowerPlatformUtility.GetPowerAutomateEndpoint(Connection.AzureEnvironment);
            var environmentName = ParameterSpecified(nameof(Environment)) ? Environment.GetName() : PowerPlatformUtility.GetDefaultEnvironment(ArmRequestHelper, Connection.AzureEnvironment)?.Name;
            var flowName = Identity.GetName();

            if (Force || ShouldContinue($"Remove flow with name '{flowName}'?", Properties.Resources.Confirm))
            {
                LogDebug($"Attempting to delete Flow with name {flowName}");
                if (ThrowExceptionIfPowerAutomateNotFound)
                {
                    try
                    {
                        // Had to add this because DELETE doesn't throw error if invalid Flow Id or Name is provided
                        LogDebug($"Retrieving Flow with name {flowName} in environment ${environmentName}");
                        var result = ArmRequestHelper.Get<Model.PowerPlatform.PowerAutomate.Flow>($"{baseUrl}/providers/Microsoft.ProcessSimple{(AsAdmin ? "/scopes/admin" : "")}/environments/{environmentName}/flows/{flowName}?api-version=2016-11-01");
                        if (result != null)
                        {
                            ArmRequestHelper.Delete($"{baseUrl}/providers/Microsoft.ProcessSimple{(AsAdmin ? "/scopes/admin" : "")}/environments/{environmentName}/flows/{flowName}?api-version=2016-11-01");
                            //RestHelper.Delete(Connection.HttpClient, $"{baseUrl}/providers/Microsoft.ProcessSimple{(AsAdmin ? "/scopes/admin" : "")}/environments/{environmentName}/flows/{flowName}?api-version=2016-11-01", AccessToken);
                            LogDebug($"Flow with name {flowName} deleted");
                        }
                    }
                    catch
                    {
                        throw new Exception($"Cannot find flow with Identity '{flowName}'");
                    }
                }
                else
                {
                    ArmRequestHelper.Delete($"{baseUrl}/providers/Microsoft.ProcessSimple{(AsAdmin ? "/scopes/admin" : "")}/environments/{environmentName}/flows/{flowName}?api-version=2016-11-01");
                    LogDebug($"Flow with name {flowName} deleted");
                }
            }
        }
    }
}