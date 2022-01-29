#UTNWS13.jcl

#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWS13.NWS13R1
then
rm ${reportdir}/UNWS13.NWS13R1
fi
if test -a ${reportdir}/UNWS13.NWS13R2
then
rm ${reportdir}/UNWS13.NWS13R2
fi

# run the program

sas ${codedir}/UTNWS13.sas -log ${reportdir}/UNWS13.NWS13R1  -mautosource
