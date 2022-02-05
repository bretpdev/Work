#UTLWO61.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWO61.LWO61R1
then
rm ${reportdir}/ULWO61.LWO61R1
fi
if test -a ${reportdir}/ULWO61.LWO61R2
then
rm ${reportdir}/ULWO61.LWO61R2
fi
if test -a ${reportdir}/ULWO61.LWO61R3
then
rm ${reportdir}/ULWO61.LWO61R3
fi
if test -a ${reportdir}/ULWO61.LWO61RZ
then
rm ${reportdir}/ULWO61.LWO61RZ
fi

# run the program

sas ${codedir}/UTLWO61.sas -log ${reportdir}/ULWO61.LWO61R1  -mautosource
