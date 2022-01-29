#UTNWS76.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/UNWS76.NWS76R1
then
rm ${reportdir}/UNWS76.NWS76R1
fi
if test -a ${reportdir}/UNWS76.NWS76RZ
then
rm ${reportdir}/UNWS76.NWS76RZ
fi
if test -a ${reportdir}/UNWS76.NWS76R2
then
rm ${reportdir}/UNWS76.NWS76R2
fi


# run the program

sas ${codedir}/UTNWS76.sas -log ${reportdir}/UNWS76.NWS76R1  -mautosource
