#UTLWA24.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWA24.LWA24R1
then
rm ${reportdir}/ULWA24.LWA24R1
fi
if test -a ${reportdir}/ULWA24.LWA24R2
then
rm ${reportdir}/ULWA24.LWA24R2
fi
if test -a ${reportdir}/ULWA24.LWA24R3
then
rm ${reportdir}/ULWA24.LWA24R3
fi
if test -a ${reportdir}/ULWA24.LWA24R4
then
rm ${reportdir}/ULWA24.LWA24R4
fi
if test -a ${reportdir}/ULWA24.LWA24RZ
then
rm ${reportdir}/ULWA24.LWA24RZ
fi

# run the program

sas ${codedir}/UTLWA24.sas -log ${reportdir}/ULWA24.LWA24R1  -mautosource
