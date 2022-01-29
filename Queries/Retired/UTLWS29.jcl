#UTLWS29.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWS29.LWS29R1
then
rm ${reportdir}/ULWS29.LWS29R1
fi
if test -a ${reportdir}/ULWS29.LWS29R2
then
rm ${reportdir}/ULWS29.LWS29R2
fi
if test -a ${reportdir}/ULWS29.LWS29RZ
then
rm ${reportdir}/ULWS29.LWS29RZ
fi

# run the program

sas ${codedir}/UTLWS29.sas -log ${reportdir}/ULWS29.LWS29R1  -mautosource
