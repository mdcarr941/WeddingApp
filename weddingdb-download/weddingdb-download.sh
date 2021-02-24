#!/bin/sh

ssh $WEDDING_SERVER\
    cd /var/lib/weddingapp/WeddingApp\
    \&\& sudo -H -u weddingapp ./WeddingApp.Cli weddingdb export-rsvps\
> $RSVP_PATH