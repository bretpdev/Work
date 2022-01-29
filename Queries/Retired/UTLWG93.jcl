#UTLWG93.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWG93.LWG93R1
then
rm ${reportdir}/ULWG93.LWG93R1
fi
if test -a ${reportdir}/ULWG93.LWG93R2
then
rm ${reportdir}/ULWG93.LWG93R2
fi
if test -a ${reportdir}/ULWG93.LWG93RZ
then
rm ${reportdir}/ULWG93.LWG93RZ
fi

# run the program

sas ${codedir}/UTLWG93.sas -log ${reportdir}/ULWG93.LWG93R1  -mautosource
