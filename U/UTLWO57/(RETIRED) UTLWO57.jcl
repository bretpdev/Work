#UTLWO57.jcl Align Forb QC Report
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWO57.LWO57R1
then
rm ${reportdir}/ULWO57.LWO57R1
fi
if test -a ${reportdir}/ULWO57.LWO57R2
then
rm ${reportdir}/ULWO57.LWO57R2
fi
if test -a ${reportdir}/ULWO57.LWO57RZ
then
rm ${reportdir}/ULWO57.LWO57RZ
fi

# run the program

sas ${codedir}/UTLWO57.sas -log ${reportdir}/ULWO57.LWO57R1  -mautosource
