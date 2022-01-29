#UTLWO83.jcl Deferment/Forbearance PPC Info
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWO83.LWO83R1
then
rm ${reportdir}/ULWO83.LWO83R1
fi
if test -a ${reportdir}/ULWO83.LWO83R2
then
rm ${reportdir}/ULWO83.LWO83R2
fi
if test -a ${reportdir}/ULWO83.LWO83R3
then
rm ${reportdir}/ULWO83.LWO83R3
fi
if test -a ${reportdir}/ULWO83.LWO83R4
then
rm ${reportdir}/ULWO83.LWO83R4
fi
if test -a ${reportdir}/ULWO83.LWO83R5
then
rm ${reportdir}/ULWO83.LWO83R5
fi
if test -a ${reportdir}/ULWO83.LWO83RZ
then
rm ${reportdir}/ULWO83.LWO83RZ
fi

# run the program

sas ${codedir}/UTLWO83.sas -log ${reportdir}/ULWO83.LWO83R1  -mautosource
