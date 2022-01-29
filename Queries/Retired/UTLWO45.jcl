#UTLWO45.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWO45.LWO45RZ
then
rm ${reportdir}/ULWO45.LWO45RZ
fi
if test -a ${reportdir}/ULWO45.LWO45R1
then
rm ${reportdir}/ULWO45.LWO45R1
fi
if test -a ${reportdir}/ULWO45.LWO45R2
then
rm ${reportdir}/ULWO45.LWO45R2
fi

# run the program

sas ${codedir}/UTLWO45.sas -log ${reportdir}/ULWO45.LWO45R1  -mautosource
