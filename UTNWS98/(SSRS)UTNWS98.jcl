#UTNWS98.jcl Web Portal Stats Fed
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/UNWS98.NWS98R1
then
rm ${reportdir}/UNWS98.NWS98R1
fi
if test -a ${reportdir}/UNWS98.NWS98RZ
then
rm ${reportdir}/UNWS98.NWS98RZ
fi
if test -a ${reportdir}/UNWS98.NWS98R2
then
rm ${reportdir}/UNWS98.NWS98R2
fi

# run the program

sas ${codedir}/UTNWS98.sas -log ${reportdir}/UNWS98.NWS98R1  -mautosource
