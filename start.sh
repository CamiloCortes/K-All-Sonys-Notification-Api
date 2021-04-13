#!/bin/bash
docker build -t k-all-sonys-notification-api:1.0 .
docker run -d -p 9094:80 --name notification-api k-all-sonys-notification-api:1.0
read -p "Press enter to continue"