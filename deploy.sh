#!/bin/bash

dotnet publish aspnet-entra-auth.csproj -c Release -o ./publish
(cd publish && zip -r ../aspnet-entra-auth-app.zip .)
az webapp deploy --resource-group mdk-demos --name aspnet-entra-auth-app --src-path aspnet-entra-auth-app.zip
