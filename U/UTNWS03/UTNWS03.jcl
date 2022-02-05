#UTNWS03.JCL

#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWS03.NWS03R1
then
rm ${reportdir}/UNWS03.NWS03R1
fi
if test -a ${reportdir}/UNWS03.NWS03RZ
then
rm ${reportdir}/UNWS03.NWS03RZ
fi
if test -a ${reportdir}/UNWS03.NWS03R2
then
rm ${reportdir}/UNWS03.NWS03R2
fi

# run the program

sas ${codedir}/UTNWS03.sas -log ${reportdir}/UNWS03.NWS03R1  -mautosource
