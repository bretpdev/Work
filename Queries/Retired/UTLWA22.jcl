#UTLWA22.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWA22.LWA22R1
then
rm ${reportdir}/ULWA22.LWA22R1
fi
if test -a ${reportdir}/ULWA22.LWA22R2
then
rm ${reportdir}/ULWA22.LWA22R2
fi
if test -a ${reportdir}/ULWA22.LWA22RZ
then
rm ${reportdir}/ULWA22.LWA22RZ
fi

# run the program

sas ${codedir}/UTLWA22.sas -log ${reportdir}/ULWA22.LWA22R1  -mautosource
