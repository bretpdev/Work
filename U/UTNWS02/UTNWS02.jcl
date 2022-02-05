#UNWS02.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/UNWS02.NWS02R1
then
rm ${reportdir}/UNWS02.NWS02R1
fi
if test -a ${reportdir}/UNWS02.NWS02RZ
then
rm ${reportdir}/UNWS02.NWS02RZ
fi
if test -a ${reportdir}/UNWS02.NWS02R2
then
rm ${reportdir}/UNWS02.NWS02R2
fi


# run the program

sas ${codedir}/UTNWS02.sas -log ${reportdir}/UNWS02.NWS02R1  -mautosource
