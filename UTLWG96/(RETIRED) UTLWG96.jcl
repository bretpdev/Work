#UTLWG96.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWG96.LWG96R1
then
rm ${reportdir}/ULWG96.LWG96R1
fi
if test -a ${reportdir}/ULWG96.LWG96R2
then
rm ${reportdir}/ULWG96.LWG96R2
fi
if test -a ${reportdir}/ULWG96.LWG96R3
then
rm ${reportdir}/ULWG96.LWG96R3
fi
if test -a ${reportdir}/ULWG96.LWG96R4
then
rm ${reportdir}/ULWG96.LWG96R4
fi
if test -a ${reportdir}/ULWG96.LWG96R5
then
rm ${reportdir}/ULWG96.LWG96R5
fi
if test -a ${reportdir}/ULWG96.LWG96R6
then
rm ${reportdir}/ULWG96.LWG96R6
fi
if test -a ${reportdir}/ULWG96.LWG96R7
then
rm ${reportdir}/ULWG96.LWG96R7
fi
if test -a ${reportdir}/ULWG96.LWG96R8
then
rm ${reportdir}/ULWG96.LWG96R8
fi
if test -a ${reportdir}/ULWG96.LWG96R9
then
rm ${reportdir}/ULWG96.LWG96R9
fi
if test -a ${reportdir}/ULWG96.LWG96R10
then
rm ${reportdir}/ULWG96.LWG96R10
fi

# run the program

sas ${codedir}/UTLWG96.sas -log ${reportdir}/ULWG96.LWG96R1  -mautosource
