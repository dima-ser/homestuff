# HomeStuff
HomeStuff is a basic home inventory management solution.

![Homestuff Main Screen](screenshot.png?raw=true "Title")
## Primary Focus
- Keep track of items/assets at home for insurance claim purposes (make, model, description, purchase price, proof of purchase, etc.)
- Simple, mobile-friendly web interface
- CSV export for backup and to help prepare data in case of a claim
  

## Secondary Features
- Import items from a .CSV file
- Ability to attach files to items (photos, receipts, manuals, etc)
- Keep track of maintenance for your items
- Password-protected so that the app can be accessible from the Internet, if so desired
- Basic value report to see the total value of items in your home/each location

# Limitations/What This Project Is NOT For
  Since the project started for my own use, these features are not of critical use for me and currently not planned to be implemented.
  
- This is not a comprehensive IT asset management tool, for that there's Snipe-IT. HomeStuff is for basic home use only. 
- No barcode/QR code labels
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
- Download updated image with `docker pull dimaser/homestuff`
- Start new container (or Reset container if using Synology NAS. That recreates it with fresh image)

# Migration
To migrate or move to another system
- Copy the entire data folder from the old system
- Install Homestuff on the new system (follow the Installation instructions above)
- Move the content of the data folder from the old system to the new system
- Run Homestuff on the new system. 
