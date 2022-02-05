#UTLWA16.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWA16.LWA16R1
then
rm ${reportdir}/ULWA16.LWA16R1
fi
if test -a ${reportdir}/ULWA16.LWA16R2
then
rm ${reportdir}/ULWA16.LWA16R2
fi
if test -a ${reportdir}/ULWA16.LWA16R3
then
rm ${reportdir}/ULWA16.LWA16R3
fi
if test -a ${reportdir}/ULWA16.LWA16R4
then
rm ${reportdir}/ULWA16.LWA16R4
fi
if test -a ${reportdir}/ULWA16.LWA16R5
then
rm ${reportdir}/ULWA16.LWA16R5
fi
if test -a ${reportdir}/ULWA16.LWA16RZ
then
rm ${reportdir}/ULWA16.LWA16RZ
fi

# run the program

sas ${codedir}/UTLWA16.sas -log ${reportdir}/ULWA16.LWA16R1  -mautosource
