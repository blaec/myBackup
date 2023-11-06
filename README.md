Simple console app for backup important data.
Service has 2 types of jobs: daily and monthly - 1st makes backup of relatively small data and 2nd - large data.
In `AppSetting.json` specified required parameters for backup:
- daily / monthly - section with daily and monthly job settings
- target - base directory for backup
- history - max size of backups (required for cleanup job)
- source - list of locations for backup

Can run as service using nssm
