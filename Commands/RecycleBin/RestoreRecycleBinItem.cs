﻿using System;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;
using Resources = PnP.PowerShell.Commands.Properties.Resources;

namespace PnP.PowerShell.Commands.RecycleBin
{
    [Cmdlet(VerbsData.Restore, "PnPRecycleBinItem")]
    [CmdletHelp("Restores the provided recycle bin item to its original location",
        Category = CmdletHelpCategory.RecycleBin)]
    [CmdletExample(
        Code = @"PS:> Restore-PnpRecycleBinItem -Identity 72e4d749-d750-4989-b727-523d6726e442",
        Remarks = "Restores the recycle bin item with Id 72e4d749-d750-4989-b727-523d6726e442 to its original location",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Get-PnPRecycleBinItem | ? -Property LeafName -like ""*.docx"" | Restore-PnpRecycleBinItem",
        Remarks = "Restores all the items in the first and second stage recycle bins to their original location of which the filename ends with the .docx extension",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Restore-PnPRecycleBinItem -RowLimit 10000",
        Remarks = "Permanently restores up to 10,000 items in the recycle bin",
        SortOrder = 4)]

    public class RestoreRecycleBinItem : PnPSharePointCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "Id of the recycle bin item or the recycle bin item object itself to restore", ValueFromPipeline = true)]
        public RecycleBinItemPipeBind Identity;

        [Parameter(Mandatory = false, HelpMessage = "If provided, no confirmation will be asked to restore the recycle bin item")]
        public SwitchParameter Force;

        [Parameter(Mandatory = false, HelpMessage = "Limits restoration to specified number of items")]
        public int RowLimit;

        protected override void ExecuteCmdlet()
        {
            if (ParameterSpecified(nameof(Identity)))
            {

                var recycleBinItem = Identity.GetRecycleBinItem(ClientContext.Site);

                if (Force || ShouldContinue(string.Format(Resources.RestoreRecycleBinItem, recycleBinItem.LeafName), Resources.Confirm))
                {
                    recycleBinItem.Restore();
                    ClientContext.ExecuteQueryRetry();
                }
            }
            else
            {
                if (ParameterSpecified(nameof(RowLimit)))
                {
                    if (Force || ShouldContinue(Resources.RestoreRecycleBinItems, Resources.Confirm))
                    {
                        RecycleBinItemCollection items = ClientContext.Site.GetRecycleBinItems(null, RowLimit, false, RecycleBinOrderBy.DeletedDate, RecycleBinItemState.None);
                        ClientContext.Load(items);
                        ClientContext.ExecuteQueryRetry();

                        items.RestoreAll();
                        ClientContext.ExecuteQueryRetry();
                    }
                }
                else
                {
                    if (Force || ShouldContinue(Resources.RestoreRecycleBinItems, Resources.Confirm))
                    {
                        ClientContext.Site.RecycleBin.RestoreAll();
                        ClientContext.ExecuteQueryRetry();
                    }
                }
            }
        }
    }
}
