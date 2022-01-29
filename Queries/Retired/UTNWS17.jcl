#UTNWS17.JCL CornerStone Interest Notifications to Borrowers in Forbearance

#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWS17.NWS17R1
then
rm ${reportdir}/UNWS17.NWS17R1
fi
if test -a ${reportdir}/UNWS17.NWS17RZ
then
rm ${reportdir}/UNWS17.NWS17RZ
fi
if test -a ${reportdir}/UNWS17.NWS17R2
then
rm ${reportdir}/UNWS17.NWS17R2
fi
if test -a ${reportdir}/UNWS17.NWS17R3
then
rm ${reportdir}/UNWS17.NWS17R3
fi
if test -a ${reportdir}/UNWS17.NWS17R4
then
rm ${reportdir}/UNWS17.NWS17R4
fi

# run the program

sas ${codedir}/UTNWS17.sas -log ${reportdir}/UNWS17.NWS17R1  -mautosource
