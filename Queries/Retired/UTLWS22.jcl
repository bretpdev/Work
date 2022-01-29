#UTLWS22.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWS22.LWS22R1
then
rm ${reportdir}/ULWS22.LWS22R1
fi
if test -a ${reportdir}/ULWS22.LWS22R2
then
rm ${reportdir}/ULWS22.LWS22R2
fi
if test -a ${reportdir}/ULWS22.LWS22RZ
then
rm ${reportdir}/ULWS22.LWS22RZ
fi

# run the program

sas ${codedir}/UTLWS22.sas -log ${reportdir}/ULWS22.LWS22R1  -mautosource
