#UTLWA33.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWA33.LWA33R1
then
rm ${reportdir}/ULWA33.LWA33R1
fi
if test -a ${reportdir}/ULWA33.LWA33R2
then
rm ${reportdir}/ULWA33.LWA33R2
fi
if test -a ${reportdir}/ULWA33.LWA33R3
then
rm ${reportdir}/ULWA33.LWA33R3
fi
if test -a ${reportdir}/ULWA33.LWA33RZ
then
rm ${reportdir}/ULWA33.LWA33RZ
fi

# run the program

sas ${codedir}/UTLWA33.sas -log ${reportdir}/ULWA33.LWA33R1  -mautosource
