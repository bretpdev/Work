#UTNWS31.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/UNWS31.NWS31R1
then
rm ${reportdir}/UNWS31.NWS31R1
fi
if test -a ${reportdir}/UNWS31.NWS31RZ
then
rm ${reportdir}/UNWS31.NWS31RZ
fi
if test -a ${reportdir}/UNWS31.NWS31R2
then
rm ${reportdir}/UNWS31.NWS31R2
fi

# run the program

sas ${codedir}/UTNWS31.sas -log ${reportdir}/UNWS31.NWS31R1  -mautosource
