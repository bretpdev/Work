#UNWS08.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/UNWS08.NWS08R1
then
rm ${reportdir}/UNWS08.NWS08R1
fi
if test -a ${reportdir}/UNWS08.NWS08RZ
then
rm ${reportdir}/UNWS08.NWS08RZ
fi
if test -a ${reportdir}/UNWS08.NWS08R2
then
rm ${reportdir}/UNWS08.NWS08R2
fi

# run the program

sas ${codedir}/UTNWS08.sas -log ${reportdir}/UNWS08.NWS08R1  -mautosource
