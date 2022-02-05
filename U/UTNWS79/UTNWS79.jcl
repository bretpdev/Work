#UTNWS79.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/UNWS79.NWS79R1
then
rm ${reportdir}/UNWS79.NWS79R1
fi
if test -a ${reportdir}/UNWS79.NWS79RZ
then
rm ${reportdir}/UNWS79.NWS79RZ
fi
if test -a ${reportdir}/UNWS79.NWS79R2
then
rm ${reportdir}/UNWS79.NWS79R2
fi
if test -a ${reportdir}/UNWS79.NWS79R3
then
rm ${reportdir}/UNWS79.NWS79R3
fi


# run the program

sas ${codedir}/UTNWS79.sas -log ${reportdir}/UNWS79.NWS79R1  -mautosource
