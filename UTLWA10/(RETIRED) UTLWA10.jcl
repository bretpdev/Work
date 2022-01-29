#UTLWA10.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWA10.LWA10R1
then
rm ${reportdir}/ULWA10.LWA10R1
fi
if test -a ${reportdir}/ULWA10.LWA10R2
then
rm ${reportdir}/ULWA10.LWA10R2
fi
if test -a ${reportdir}/ULWA10.LWA10R3
then
rm ${reportdir}/ULWA10.LWA10R3
fi
if test -a ${reportdir}/ULWA10.LWA10RZ
then
rm ${reportdir}/ULWA10.LWA10RZ
fi

# run the program

sas ${codedir}/UTLWA10.sas -log ${reportdir}/ULWA10.LWA10R1  -mautosource
