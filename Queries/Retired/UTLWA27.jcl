#UTLWA27.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWA27.LWA27R1
then
rm ${reportdir}/ULWA27.LWA27R1
fi
if test -a ${reportdir}/ULWA27.LWA27R2
then
rm ${reportdir}/ULWA27.LWA27R2
fi
if test -a ${reportdir}/ULWA27.LWA27RZ
then
rm ${reportdir}/ULWA27.LWA27RZ
fi

# run the program

sas ${codedir}/UTLWA27.sas -log ${reportdir}/ULWA27.LWA27R1  -mautosource
