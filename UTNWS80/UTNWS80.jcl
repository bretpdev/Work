#UTNWS80.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/UNWS80.NWS80R1
then
rm ${reportdir}/UNWS80.NWS80R1
fi
if test -a ${reportdir}/UNWS80.NWS80RZ
then
rm ${reportdir}/UNWS80.NWS80RZ
fi
if test -a ${reportdir}/UNWS80.NWS80R2
then
rm ${reportdir}/UNWS80.NWS80R2
fi
if test -a ${reportdir}/UNWS80.NWS80R3
then
rm ${reportdir}/UNWS80.NWS80R3
fi


# run the program

sas ${codedir}/UTNWS80.sas -log ${reportdir}/UNWS80.NWS80R1  -mautosource
