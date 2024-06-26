---
Module Name: PnP.PowerShell
schema: 2.0.0
applicable: SharePoint Online
online version: https://pnp.github.io/powershell/cmdlets/Add-PnPFolderUserSharingLink.html
external help file: PnP.PowerShell.dll-Help.xml
title: Add-PnPFolderUserSharingLink
---
  
# Add-PnPFolderUserSharingLink

## SYNOPSIS
Creates a sharing link to share a folder with a list of specified users.

## SYNTAX

```powershell
Add-PnPFolderUserSharingLink -Folder <FolderPipeBind> -ShareType <PnP.Core.Model.Security.ShareType> -Users <String[]> [-Connection <PnPConnection>] 
```

## DESCRIPTION

Creates a new user sharing link for a folder.

## EXAMPLES

### EXAMPLE 1
```powershell
Add-PnPFolderUserSharingLink -Folder "/sites/demo/Shared Documents/Test" -Users "john@contoso.onmicrosoft.com","jane@contoso.onmicrosoft.com"
```

This will create an user sharing link for `Test` folder in the `Shared Documents` library which will be viewable to specified users in the organization.

### EXAMPLE 2
```powershell
Add-PnPFolderUserSharingLink -Folder "/sites/demo/Shared Documents/Test" -ShareType Edit -Users "john@contoso.onmicrosoft.com","jane@contoso.onmicrosoft.com"
```

This will create an user sharing link for `Test` folder in the `Shared Documents` library which will be editable by specified users in the organization.

## PARAMETERS

### -Connection
Optional connection to be used by the cmdlet. Retrieve the value for this parameter by either specifying -ReturnConnection on Connect-PnPOnline or by executing Get-PnPConnection.

```yaml
Type: PnPConnection
Parameter Sets: (All)

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Folder
The folder in the site

```yaml
Type: FolderPipeBind
Parameter Sets: (All)

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ShareType
The type of sharing that you want to, i.e do you want to enable people in your organization to view the shared content or also edit the content?

`Review` and `BlocksDownload` values are not supported.

```yaml
Type: PnP.Core.Model.Security.ShareType
Parameter Sets: (All)

Required: False
Position: Named
Default value: View
Accept pipeline input: False
Accept wildcard characters: False
```

### -Users
The UPN(s) of the user(s) to with whom you would like to share the folder.

```yaml
Type: String[]
Parameter Sets: (All)

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

## RELATED LINKS

[Microsoft 365 Patterns and Practices](https://aka.ms/m365pnp)
