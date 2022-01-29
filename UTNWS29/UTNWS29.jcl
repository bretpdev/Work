#UTNWS29.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/UNWS29.NWS29R1
then
rm ${reportdir}/UNWS29.NWS29R1
fi
if test -a ${reportdir}/UNWS29.NWS29RZ
then
rm ${reportdir}/UNWS29.NWS29RZ
fi
if test -a ${reportdir}/UNWS29.NWS29R2
then
rm ${reportdir}/UNWS29.NWS29R2
fi

# run the program

sas ${codedir}/UTNWS29.sas -log ${reportdir}/UNWS29.NWS29R1  -mautosource
