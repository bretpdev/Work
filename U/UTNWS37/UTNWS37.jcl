#UTNWS37.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/UNWS37.NWS37R1
then
rm ${reportdir}/UNWS37.NWS37R1
fi
if test -a ${reportdir}/UNWS37.NWS37RZ
then
rm ${reportdir}/UNWS37.NWS37RZ
fi
if test -a ${reportdir}/UNWS37.NWS37R2
then
rm ${reportdir}/UNWS37.NWS37R2
fi

# run the program

sas ${codedir}/UTNWS37.sas -log ${reportdir}/UNWS37.NWS37R1  -mautosource
