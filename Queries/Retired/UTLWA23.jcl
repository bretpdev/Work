#UTLWA23.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWA23.LWA23R1
then
rm ${reportdir}/ULWA23.LWA23R1
fi
if test -a ${reportdir}/ULWA23.LWA23R2
then
rm ${reportdir}/ULWA23.LWA23R2
fi
if test -a ${reportdir}/ULWA23.LWA23R3
then
rm ${reportdir}/ULWA23.LWA23R3
fi
if test -a ${reportdir}/ULWA23.LWA23RZ
then
rm ${reportdir}/ULWA23.LWA23RZ
fi

# run the program

sas ${codedir}/UTLWA23.sas -log ${reportdir}/ULWA23.LWA23R1  -mautosource
