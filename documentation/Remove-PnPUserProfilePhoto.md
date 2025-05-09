---
Module Name: PnP.PowerShell
title: Remove-PnPUserProfilePhoto
schema: 2.0.0
applicable: SharePoint Online
external help file: PnP.PowerShell.dll-Help.xml
online version: https://pnp.github.io/powershell/cmdlets/Remove-PnPUserProfilePhoto.html
---
 
# Remove-PnPUserProfilePhoto

## SYNOPSIS

**Required Permissions**

  * Microsoft Graph API: One of ProfilePhoto.ReadWrite.All, User.ReadWrite or User.ReadWrite.All

Remove the profile picture of a user.

## SYNTAX

```powershell
Remove-PnPUserProfilePhoto -Identity <AzureADUserPipeBind> [-Connection <PnPConnection>] 
```

## DESCRIPTION
Notice that this cmdlet will immediately return but it can take a few hours before the changes are reflected in profile picture of the user everywhere in M365.

## EXAMPLES

### EXAMPLE 1
```powershell
Remove-PnPUserProfilePhoto -Identity "john@contoso.onmicrosoft.com"
```
Removes the picture for the user with user principal name `john`

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

### -Identity
The identity of the user to remove. This can be the UPN, the GUID or an instance of the user.

```yaml
Type: AzureADUserPipeBind
Parameter Sets: (All)

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

## RELATED LINKS

[Microsoft 365 Patterns and Practices](https://aka.ms/m365pnp)
