#UTNWS71.jcl

#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWS71.NWS71R1
then
rm ${reportdir}/UNWS71.NWS71R1
fi
if test -a ${reportdir}/UNWS71.NWS71R2
then
rm ${reportdir}/UNWS71.NWS71R2
fi

# run the program

sas ${codedir}/UTNWS71.sas -log ${reportdir}/UNWS71.NWS71R1  -mautosource
