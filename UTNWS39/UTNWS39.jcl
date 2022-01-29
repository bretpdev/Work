#UTNWS39.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/UNWS39.NWS39R1
then
rm ${reportdir}/UNWS39.NWS39R1
fi
if test -a ${reportdir}/UNWS39.NWS39RZ
then
rm ${reportdir}/UNWS39.NWS39RZ
fi
if test -a ${reportdir}/UNWS39.NWS39R2
then
rm ${reportdir}/UNWS39.NWS39R2
fi


# run the program

sas ${codedir}/UTNWS39.sas -log ${reportdir}/UNWS39.NWS39R1  -mautosource
