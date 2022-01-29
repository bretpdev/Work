#UTNWS22.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/UNWS22.NWS22R1
then
rm ${reportdir}/UNWS22.NWS22R1
fi
if test -a ${reportdir}/UNWS22.NWS22RZ
then
rm ${reportdir}/UNWS22.NWS22RZ
fi
if test -a ${reportdir}/UNWS22.NWS22R2
then
rm ${reportdir}/UNWS22.NWS22R2
fi


# run the program

sas ${codedir}/UTNWS22.sas -log ${reportdir}/UNWS22.NWS22R1  -mautosource
