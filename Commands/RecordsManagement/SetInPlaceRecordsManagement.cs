﻿using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System.Linq.Expressions;
using System;
using System.Linq;
using System.Collections.Generic;
using PnP.PowerShell.Commands.Base;

namespace PnP.PowerShell.Commands.RecordsManagement
{
    [Cmdlet(VerbsCommon.Set, "PnPInPlaceRecordsManagement")]
    [CmdletHelp("Activates or deactivates in the place records management feature.",
        Category = CmdletHelpCategory.RecordsManagement)]
    [CmdletExample(
        Code = @"PS:> Set-PnPInPlaceRecordsManagement -Enabled $true",
        Remarks = "Activates In Place Records Management",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Set-PnPInPlaceRecordsManagement -Enabled $false",
        Remarks = "Deactivates In Place Records Management",
        SortOrder = 2)]
    public class SetInPlaceRecordsManagement : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, ParameterSetName = "Enable or Disable")]
        public bool Enabled;

        protected override void ExecuteCmdlet()
        {
            if (Enabled)
            {
                ClientContext.Site.ActivateInPlaceRecordsManagementFeature();
            }
            else
            {
                ClientContext.Site.DisableInPlaceRecordsManagementFeature();
            }
        }

    }

}
