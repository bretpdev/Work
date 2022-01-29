#UTLWG94.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWG94.LWG94R1
then
rm ${reportdir}/ULWG94.LWG94R1
fi
if test -a ${reportdir}/ULWG94.LWG94R2
then
rm ${reportdir}/ULWG94.LWG94R2
fi
if test -a ${reportdir}/ULWG94.LWG94RZ
then
rm ${reportdir}/ULWG94.LWG94RZ
fi

# run the program

sas ${codedir}/UTLWG94.sas -log ${reportdir}/ULWG94.LWG94R1  -mautosource
