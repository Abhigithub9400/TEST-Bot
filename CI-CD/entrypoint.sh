#!/bin/bash

dotnet /app/build/MediAssist.UI.dll &
apachectl -D FOREGROUND
