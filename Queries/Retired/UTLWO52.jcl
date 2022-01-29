#UTLWO52.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWO52.LWO52RZ
then
rm ${reportdir}/ULWO52.LWO52RZ
fi
if test -a ${reportdir}/ULWO52.LWO52R1
then
rm ${reportdir}/ULWO52.LWO52R1
fi
if test -a ${reportdir}/ULWO52.LWO52R2
then
rm ${reportdir}/ULWO52.LWO52R2
fi

# run the program

sas ${codedir}/UTLWO52.sas -log ${reportdir}/ULWO52.LWO52R1  -mautosource
