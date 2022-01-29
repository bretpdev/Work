#UTLWA25.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWA25.LWA25R1
then
rm ${reportdir}/ULWA25.LWA25R1
fi
if test -a ${reportdir}/ULWA25.LWA25R2
then
rm ${reportdir}/ULWA25.LWA25R2
fi
if test -a ${reportdir}/ULWA25.LWA25RZ
then
rm ${reportdir}/ULWA25.LWA25RZ
fi

# run the program

sas ${codedir}/UTLWA25.sas -log ${reportdir}/ULWA25.LWA25R1  -mautosource
