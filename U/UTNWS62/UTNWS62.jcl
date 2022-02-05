#UTNWS62.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/UNWS62.NWS62R1
then
rm ${reportdir}/UNWS62.NWS62R1
fi
if test -a ${reportdir}/UNWS62.NWS62RZ
then
rm ${reportdir}/UNWS62.NWS62RZ
fi
if test -a ${reportdir}/UNWS62.NWS62R2
then
rm ${reportdir}/UNWS62.NWS62R2
fi
if test -a ${reportdir}/UNWS62.NWS62R3
then
rm ${reportdir}/UNWS62.NWS62R3
fi

# run the program

sas ${codedir}/UTNWS62.sas -log ${reportdir}/UNWS62.NWS62R1  -mautosource
