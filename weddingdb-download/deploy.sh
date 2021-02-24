#/bin/sh
set -e

SERVER=$1
TMPDIR=$(ssh $SERVER mktemp -d /tmp/wedding-download-XXXXXX)
scp weddingdb-download.{sh,service,timer} $SERVER:$TMPDIR
ssh -t $SERVER sudo mv $TMPDIR/\* /etc/systemd/system/\
    \&\& rm -rf $TMPDIR\
    \&\& sudo systemctl daemon-reload\
    \&\& sudo systemctl enable weddingdb-download.timer\
    \&\& sudo systemctl start weddingdb-download.service