#UTLWO58.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWO58.LWO58R1
then
rm ${reportdir}/ULWO58.LWO58R1
fi
if test -a ${reportdir}/ULWO58.LWO58R2
then
rm ${reportdir}/ULWO58.LWO58R2
fi
if test -a ${reportdir}/ULWO58.LWO58R3
then
rm ${reportdir}/ULWO58.LWO58R3
fi
if test -a ${reportdir}/ULWO58.LWO58RZ
then
rm ${reportdir}/ULWO58.LWO58RZ
fi

# run the program

sas ${codedir}/UTLWO58.sas -log ${reportdir}/ULWO58.LWO58R1  -mautosource
