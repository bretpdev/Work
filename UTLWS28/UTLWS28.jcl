#UTLWS28.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWS28.LWS28R1
then
rm ${reportdir}/ULWS28.LWS28R1
fi
if test -a ${reportdir}/ULWS28.LWS28R2
then
rm ${reportdir}/ULWS28.LWS28R2
fi
if test -a ${reportdir}/ULWS28.LWS28RZ
then
rm ${reportdir}/ULWS28.LWS28RZ
fi

# run the program

sas ${codedir}/UTLWS28.sas -log ${reportdir}/ULWS28.LWS28R1  -mautosource
