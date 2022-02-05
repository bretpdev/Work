#UTNWS74.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/UNWS74.NWS74R1
then
rm ${reportdir}/UNWS74.NWS74R1
fi
if test -a ${reportdir}/UNWS74.NWS74RZ
then
rm ${reportdir}/UNWS74.NWS74RZ
fi
if test -a ${reportdir}/UNWS74.NWS74R2
then
rm ${reportdir}/UNWS74.NWS74R2
fi

# run the program

sas ${codedir}/UTNWS74.sas -log ${reportdir}/UNWS74.NWS74R1  -mautosource
