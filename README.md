# HomeStuff
HomeStuff is a basic home inventory management solution.

## Primary Focus
- Keep track of items/assets at home for insurance claim purposes (make, model, description, purchase price, proof of purchase, etc.)
- Simple, mobile-friendly web interface
- CSV export for backup and to help prepare data in case of a claim
  

## Secondary Features
- Import items from a CSV file
- Ability to attach files to items (photos, receipts, manuals, etc)
- Keep track of maintenance for your items
- Password-protected so that the app can be accessible from the Internet, if so desired

# Limitations/What This Project Is NOT For
- This is not a comprehensive IT asset management tool, for that there's Snipe-IT. HomeStuff is for basic home use only. 
- No labels/barcodes/QR codes
- No multi-user support, one user only
- US-centric (no multi-currency or localization support).

# Installation (Docker)
Docker image hosted on DockerHub (private repo). To pull, use `docker pull dimaser/homestuff` You may need to first log in to DockerHub since it's a private repo. To log in, use `docker login`

### Docker parameters
`-p [any port you want]:80`
This is the port on Docker host you want to run the application on.

`-v [/path/to/yourdatadir]:/db`
Directory on the host where you want to store the application data (database and attachments). Keep this backed up.

`-e TZ=[your time zone]`
A string representing your time zone as per https://en.wikipedia.org/wiki/List_of_tz_database_time_zones (e.g., `America/Los_Angeles`). Used to make sure the application handles dates/time correctly.

### First run/config
You'll be asked to provide an admin password. Should you forget this password in the future, simply delete the `password.txt` file in the app's data directory to reset the password.

# Upgrade
To upgrade
- Stop container
- Clear/reset container
- Download updated image with `docker pull dimaser/homestuff`
- Start container

If application is still showing the old version, you may need to stop, reset and restart the container again.
