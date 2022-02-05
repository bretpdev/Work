#UTLWO96.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWO96.LWO96R1
then
rm ${reportdir}/ULWO96.LWO96R1
fi
if test -a ${reportdir}/ULWO96.LWO96R2
then
rm ${reportdir}/ULWO96.LWO96R2
fi
if test -a ${reportdir}/ULWO96.LWO96R3
then
rm ${reportdir}/ULWO96.LWO96R3
fi
if test -a ${reportdir}/ULWO96.LWO96RZ
then
rm ${reportdir}/ULWO96.LWO96RZ
fi

# run the program

sas ${codedir}/UTLWO96.sas -log ${reportdir}/ULWO96.LWO96R1  -mautosource
