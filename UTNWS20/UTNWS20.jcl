#UTNWS20.JCL 

#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWS20.NWS20R1
then
rm ${reportdir}/UNWS20.NWS20R1
fi
if test -a ${reportdir}/UNWS20.NWS20RZ
then
rm ${reportdir}/UNWS20.NWS20RZ
fi
if test -a ${reportdir}/UNWS20.NWS20R2
then
rm ${reportdir}/UNWS20.NWS20R2
fi
if test -a ${reportdir}/UNWS20.NWS20R3
then
rm ${reportdir}/UNWS20.NWS20R3
fi

# run the program

sas ${codedir}/UTNWS20.sas -log ${reportdir}/UNWS20.NWS20R1  -mautosource
