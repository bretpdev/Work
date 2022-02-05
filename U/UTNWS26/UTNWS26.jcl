#UTNWS26.jcl

#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWS26.NWS26R1
then
rm ${reportdir}/UNWS26.NWS26R1
fi
if test -a ${reportdir}/UNWS26.NWS26R2
then
rm ${reportdir}/UNWS26.NWS26R2
fi

# run the program

sas ${codedir}/UTNWS26.sas -log ${reportdir}/UNWS26.NWS26R1  -mautosource
