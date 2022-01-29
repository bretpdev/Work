#UTLWO44.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWO44.LWO44R1
then
rm ${reportdir}/ULWO44.LWO44R1
fi
if test -a ${reportdir}/ULWO44.LWO44R2
then
rm ${reportdir}/ULWO44.LWO44R2
fi
if test -a ${reportdir}/ULWO44.LWO44RZ
then
rm ${reportdir}/ULWO44.LWO44RZ
fi

# run the program

sas ${codedir}/UTLWO44.sas -log ${reportdir}/ULWO44.LWO44R1  -mautosource
