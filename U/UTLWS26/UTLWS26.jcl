#UTLWS26.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWS26.LWS26RZ
then
rm ${reportdir}/ULWS26.LWS26RZ
fi
if test -a ${reportdir}/ULWS26.LWS26R1
then
rm ${reportdir}/ULWS26.LWS26R1
fi
if test -a ${reportdir}/ULWS26.LWS26R2
then
rm ${reportdir}/ULWS26.LWS26R2
fi

# run the program

sas ${codedir}/UTLWS26.sas -log ${reportdir}/ULWS26.LWS26R1  -mautosource
